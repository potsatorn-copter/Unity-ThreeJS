import * as THREE from 'three';

export function setupLights(scene: THREE.Scene, lightPosition: THREE.Vector3) {
  const ambientLight = new THREE.AmbientLight(0xffffff, 0.2);
  scene.add(ambientLight);

  const directionalLight = new THREE.DirectionalLight(0xffffff, 0.6);
  directionalLight.position.copy(lightPosition);
  scene.add(directionalLight);

  const hemiLight = new THREE.HemisphereLight(0xffffff, 0x444444, 0.4);
  hemiLight.position.set(0, 50, 0);
  scene.add(hemiLight);

}