import * as THREE from 'three';

// ฟังก์ชันสำหรับตั้งค่าแสงใน scene
export function setupLights(scene: THREE.Scene, lightPosition: THREE.Vector3) {
  const ambientLight = new THREE.AmbientLight(0xffffff, 0.2);
  scene.add(ambientLight);

  const directionalLight = new THREE.DirectionalLight(0xffffff, 0.7);
  directionalLight.position.copy(lightPosition); // ใช้ตำแหน่งที่ส่งเข้ามา
  scene.add(directionalLight);
}
  
