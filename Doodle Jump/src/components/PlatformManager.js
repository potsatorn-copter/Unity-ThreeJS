import * as THREE from 'three';

// เก็บประวัติแพลตฟอร์มล่าสุดไว้เช็คบาลานซ์
let platformHistory = [];

/** ✅ เช็คทับกันแบบ AABB (กันแน่นขึ้น) */
function isOverlapping(x, y, z, platforms) {
  const halfSize = 5;      // 10/2
  const halfDepth = 5;     // 10/2
  const halfHeight = 0.35; // 0.7/2

  for (let other of platforms) {
    if (
      Math.abs(x - other.position.x) < halfSize * 2 &&              // กว้าง
      Math.abs(y - other.position.y) < halfHeight * 2 + 3 &&        // Y buffer ↑ (เดิม +2 → +3)
      Math.abs(z - other.position.z) < halfDepth * 2 + 2            // Z buffer ↑ (+2)
    ) {
      return true;
    }
  }
  return false;
}

/** ฟังก์ชันสำหรับสร้าง platform */
export function createPlatform(x, y, z, color, scene, platforms) {
  const platform = new THREE.Mesh(
    new THREE.BoxGeometry(10, 0.7, 10),
    new THREE.MeshStandardMaterial({ color })
  );
  platform.position.set(x, y, z);
  platform.userData.isDestructible = color === '#ff0000';
  scene.add(platform);
  platforms.push(platform);

  // เก็บประวัติสีไว้บาลานซ์
  platformHistory.push(color === '#ff0000' ? 'red' : 'blue');
  if (platformHistory.length > 20) platformHistory.shift();

  return platform;
}

/** รีเซ็ตแพลตฟอร์ม (ล็อค X = 0) */
export function resetPlatforms(scene, platforms, platformPositions, highestPlatformY, minHeight) {
  platforms.forEach(p => scene.remove(p));
  platforms.length = 0;

  platformPositions.forEach(pos => {
    createPlatform(0, pos.y, pos.z, pos.color, scene, platforms); // ✅ fix X = 0
  });

  highestPlatformY = platformPositions[platformPositions.length - 1].y;
  minHeight = 0;
  return { highestPlatformY, minHeight };
}

/** รีไซเคิลแพลตฟอร์ม */
export function recyclePlatforms(scene, camera, platforms, highestPlatformY, maxGap, createBluePlatformNearby) {
  platforms.forEach(platform => {
    if (platform.position.y < camera.position.y - 20) {
      for (let i = 0; i < 3; i++) {
        highestPlatformY = createNewPlatform(highestPlatformY, maxGap, platforms, scene, createBluePlatformNearby);
      }
      scene.remove(platform);
      platforms.splice(platforms.indexOf(platform), 1);
    }
  });
  return highestPlatformY;
}

/** สร้างแพลตฟอร์มใหม่ (บาลานซ์ + กันทับ + กระจาย Z) */
export function createNewPlatform(highestPlatformY, maxGap, platforms, scene, createBluePlatformNearby) {
  const minExtraGap = 3;     // เพิ่มช่องว่างแกน Y
  const minZSpacing = 10;    // ✅ ต้องห่างแกน Z ขั้นต่ำ (เข้มขึ้น)
  let newY = highestPlatformY + maxGap + minExtraGap;

  let attempts = 0;
  let isPositionValid = false;
  const newX = 0; // ✅ fix X
  let newZ = 0;

  while (!isPositionValid && attempts < 120) {
    newZ = (Math.random() - 0.5) * 40; // ✅ กระจาย Z กว้างขึ้น (-20..20)

    // กันชิดใน Z เมื่ออยู่ชั้นใกล้กัน (±6 หน่วยใน Y)
    const tooCloseZSameLayer = platforms.some(
      p => Math.abs(p.position.y - newY) < 6 && Math.abs(p.position.z - newZ) < minZSpacing
    );

    isPositionValid = !isOverlapping(newX, newY, newZ, platforms) && !tooCloseZSameLayer;
    attempts++;
  }

  if (!isPositionValid) return highestPlatformY;

  // ====== บาลานซ์แดง/น้ำเงิน ======
  const recent = platformHistory.slice(-8);
  const redCount = recent.filter(c => c === 'red').length;
  const lastTwo = recent.slice(-2);
  let chanceRed = 0.2;

  if (redCount === 0 && recent.length >= 6) {
    createPlatform(newX, newY, newZ, '#ff0000', scene, platforms);
    return newY;
  }
  if (redCount >= 3) chanceRed = 0.05;
  else if (redCount === 0) chanceRed = 0.35;

  if (lastTwo.every(c => c === 'red')) {
    createPlatform(newX, newY, newZ, '#0369a1', scene, platforms);
    return newY;
  }

  const newColor = Math.random() < chanceRed ? '#ff0000' : '#0369a1';
  createPlatform(newX, newY, newZ, newColor, scene, platforms);

  // ถ้าออกแดงและสัดส่วนแดงเริ่มเยอะ → สร้างน้ำเงินช่วย แต่ต้องขยับห่างจริง ๆ
  if (newColor === '#ff0000' && redCount >= 2) {
    createBluePlatformNearby(newY, newZ, scene, platforms);
  }

  return newY;
}

/** สร้างแพลตฟอร์มสีน้ำเงินใกล้ ๆ (แต่ไม่ชนและไม่ชิด Z) */ 
export function createBluePlatformNearby(y, z, scene, platforms) {
  const minZSpacing = 10;  // ✅ เข้มขึ้นเท่าฟังก์ชันหลัก
  let attempts = 0;
  let isPositionValid = false;
  const newX = 0; // ✅ fix X = 0
  let newY = y + 3; // ✅ ขยับสูงขึ้นเล็กน้อย
  let newZ = 0;

  while (!isPositionValid && attempts < 80) {
    newZ = (Math.random() - 0.5) * 40; // ✅ ช่วง Z กว้างขึ้น

    const tooCloseZSameLayer = platforms.some(
      p =>
        Math.abs(p.position.y - newY) < 6 &&
        Math.abs(p.position.z - newZ) < minZSpacing
    );

    // ✅ ป้องกันไม่ให้สีตรงข้ามซ้อนหรือใกล้เกินไป
    const tooCloseOppositeColor = platforms.some(
      p =>
        p.userData.isDestructible && // คือแพลตฟอร์มสีแดง
        Math.abs(p.position.y - newY) < 4 && // ระยะ Y ใกล้เกินไป
        Math.abs(p.position.z - newZ) < 10   // ระยะ Z ใกล้เกินไป
    );

    isPositionValid =
      !isOverlapping(newX, newY, newZ, platforms) &&
      !tooCloseZSameLayer &&
      !tooCloseOppositeColor;

    attempts++;
  }

  if (isPositionValid) {
    createPlatform(newX, newY, newZ, '#0369a1', scene, platforms);
  }
}


/** สร้างแพลตฟอร์มเริ่มต้น (ล็อค X = 0) */
export function createInitialPlatforms(platformPositions, scene, platforms, highestPlatformY, minHeight, redPlatformCount, createBluePlatformNearby) {
  platformPositions.forEach(pos => {
    createPlatform(0, pos.y, pos.z, pos.color, scene, platforms); // ✅ fix X = 0
    if (pos.color === '#ff0000') redPlatformCount++;
  });
  highestPlatformY = platformPositions[platformPositions.length - 1].y;
  minHeight = 0;
  return { highestPlatformY, minHeight };
}

/** ตรวจสอบการชน */
export function checkPlatformCollision(
  characterRef,
  platforms,
  scene,
  highestPlatformY,
  maxGap,
  minHeightRef,
  isJumping,
  scoreRef,
  jumpVelocity,
  gravity,
  jumpSoundRef,
  breakSoundRef,
  updateDebugInfo,
  endGame,
  createNewPlatform
) {
  platforms.forEach(platform => {
    const box = platform.geometry;
    if (
      characterRef.current.position.y <= platform.position.y + 0.5 &&
      characterRef.current.position.y >= platform.position.y + 0.25 &&
      characterRef.current.position.x > platform.position.x - box.parameters.width / 2 &&
      characterRef.current.position.x < platform.position.x + box.parameters.width / 2 &&
      characterRef.current.position.z > platform.position.z - box.parameters.depth / 2 &&
      characterRef.current.position.z < platform.position.z + box.parameters.depth / 2 &&
      characterRef.current.velocity.y < 0
    ) {
      if (platform.userData.isDestructible) {
        scene.remove(platform);
        platforms = platforms.filter(p => p !== platform);
        isJumping.current = true;
        for (let i = 0; i < 2; i++) {
          highestPlatformY = createNewPlatform(highestPlatformY, maxGap, platforms, scene, createBluePlatformNearby);
        }
        if (breakSoundRef.current) {
          if (breakSoundRef.current.isPlaying) breakSoundRef.current.stop();
          if (typeof breakSoundRef.current.currentTime === 'number') breakSoundRef.current.currentTime = 0;
          breakSoundRef.current.play();
        }
      } else {
        characterRef.current.position.y = platform.position.y + 0.25;
        characterRef.current.velocity.y = jumpVelocity;
        isJumping.current = false;

        if (jumpSoundRef.current) {
          if (jumpSoundRef.current.isPlaying) jumpSoundRef.current.stop();
          if (typeof jumpSoundRef.current.currentTime === 'number') jumpSoundRef.current.currentTime = 0;
          jumpSoundRef.current.play();
        }
      }

      if (characterRef.current.position.y > minHeightRef.current) {
        minHeightRef.current = characterRef.current.position.y - 10;
      }
    }
  });
}
