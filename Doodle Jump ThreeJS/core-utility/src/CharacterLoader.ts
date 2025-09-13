import * as THREE from 'three';
import { MTLLoader } from 'three/examples/jsm/loaders/MTLLoader.js';
import { OBJLoader } from 'three/examples/jsm/loaders/OBJLoader.js';
import { FBXLoader } from 'three/examples/jsm/loaders/FBXLoader.js';

// ฟังก์ชันสำหรับโหลดโมเดลตัวละคร FBX 
export function loadFBXCharacter(path: string, scene: THREE.Scene, sounds: any, callback: (character: THREE.Group) => void, characterName: string) {
  const fbxLoader = new FBXLoader(); // สร้างอินสแตนซ์ของ FBXLoader
  fbxLoader.load(path, (object: any) => { // โหลดไฟล์ FBX จาก path ที่กำหนด
    callback(object); // เรียก callback เมื่อโมเดลถูกโหลดเสร็จ
  });
}

// ฟังก์ชันสำหรับโหลดโมเดลตัวละคร OBJ
export function loadOBJCharacter(mtlPath: string, objPath: string, scene: THREE.Scene, sounds: any, callback: (character: THREE.Group) => void) {
  const mtlLoader = new MTLLoader(); // สร้างอินสแตนซ์ของ MTLLoader
  mtlLoader.load(mtlPath, (materials: any) => { // โหลดไฟล์ MTL จาก mtlPath ที่กำหนด
    materials.preload(); // preload materials
    const objLoader = new OBJLoader(); // สร้างอินสแตนซ์ของ OBJLoader
    objLoader.setMaterials(materials); // กำหนด materials ให้กับ OBJLoader
    objLoader.load(objPath, (object: any) => { // โหลดไฟล์ OBJ จาก objPath ที่กำหนด
      callback(object); // เรียก callback เมื่อโมเดลถูกโหลดเสร็จ
    });
  });
}