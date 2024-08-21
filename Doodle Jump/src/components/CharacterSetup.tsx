import * as THREE from 'three';
import { loadFBXCharacter, loadOBJCharacter } from 'core-utility/src/CharacterLoader';

// ฟังก์ชันสำหรับเพิ่มเสียงให้กับตัวละคร
export function addSounds(character: THREE.Group, sounds: any) {
  if (sounds.jumpSound instanceof THREE.Object3D) {
    character.add(sounds.jumpSound);
  }
  if (sounds.breakSound instanceof THREE.Object3D) {
    character.add(sounds.breakSound);
  }
  if (sounds.winSound instanceof THREE.Object3D) {
    character.add(sounds.winSound);
  }
  if (sounds.gameOverSound instanceof THREE.Object3D) {
    character.add(sounds.gameOverSound);
  }
}

// ฟังก์ชันสำหรับตั้งค่าตัวละคร
export function setupCharacter(character: THREE.Group, sounds: any, scene: THREE.Scene, callback: (character: THREE.Group) => void, characterName: string) {
  character.position.set(0, 6, 0);
  character.rotation.y = Math.PI / 2;
  scene.add(character);
  addSounds(character, sounds);
  callback(character);
  console.log(`Character ${characterName} loaded`);
  console.log(`Character ${characterName} position:`, character.position);
}

// ฟังก์ชันสำหรับโหลดตัวละคร Doodle
function loadDoodle(scene: THREE.Scene, sounds: any, callback: (character: THREE.Group) => void) {
  loadOBJCharacter(
    '/3D_and_Picture/DoodleJump.mtl',
    '/3D_and_Picture/DoodleJump.obj',
    scene,
    sounds,
    character => {
      character.scale.set(1, 1, 1); // ตั้งค่า scale ของตัวละคร Doodle
      setupCharacter(character, sounds, scene, callback, 'Doodle');
    }
  );
}

// ฟังก์ชันสำหรับโหลดตัวละคร A
function loadCharacterA(scene: THREE.Scene, sounds: any, callback: (character: THREE.Group) => void) {
  loadFBXCharacter(
    '/3D_and_Picture/A (Russian Alphabet Lore).fbx',
    scene,
    sounds,
    character => {
      character.scale.set(5, 3, 5); // ตั้งค่า scale ของตัวละคร A
      setupCharacter(character, sounds, scene, callback, 'A');
    },
    'A'
  );
}

// ฟังก์ชันสำหรับโหลดตัวละคร D
function loadCharacterD(scene: THREE.Scene, sounds: any, callback: (character: THREE.Group) => void) {
  loadFBXCharacter(
    '/3D_and_Picture/D.fbx',
    scene,
    sounds,
    character => {
      character.scale.set(5, 3, 5); // ตั้งค่า scale ของตัวละคร D 
      setupCharacter(character, sounds, scene, callback, 'D');
    },
    'D'
  );
}

// ฟังก์ชันสำหรับโหลดตัวละครและเพิ่มตัวละครลงใน scene
export function loadCharacter(
  scene: THREE.Scene,
  sounds: any,
  callback: (character: THREE.Group) => void,
  selectedCharacter: string
) {
  console.log(`Selected Character in loadCharacter: ${selectedCharacter}`);

  switch (selectedCharacter) {
    case 'Doodle':
      loadDoodle(scene, sounds, callback);
      break;
    case 'A':
      loadCharacterA(scene, sounds, callback);
      break;
    case 'D':
      loadCharacterD(scene, sounds, callback);
      break;
    default:
      console.error(`Unknown character: ${selectedCharacter}`);
      break;
  }
}