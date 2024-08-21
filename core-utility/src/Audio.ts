import React from 'react';
import { PositionalAudio, AudioLoader, AudioListener } from 'three';



// ฟังก์ชันสำหรับการโหลดเสียงและตั้งค่าเสียง
function loadAudio(listener: AudioListener, file: string, setAudio: (audio: PositionalAudio) => void) {
  const audioLoader = new AudioLoader();
  const audio = new PositionalAudio(listener);

  audioLoader.load(file, (buffer: AudioBuffer) => {
    audio.setBuffer(buffer);
    audio.setRefDistance(30);
    setAudio(audio);
  });
}

// ฟังก์ชันสำหรับโหลดเสียงต่างๆ
export function loadSounds(
  listener: AudioListener,
  jumpSoundRef: React.MutableRefObject<PositionalAudio | undefined>,
  breakSoundRef: React.MutableRefObject<PositionalAudio | undefined>,
  winSoundRef: React.MutableRefObject<PositionalAudio | undefined>,
  gameOverSoundRef: React.MutableRefObject<PositionalAudio | undefined>
) {
  loadAudio(listener, 'Sound/cartoon-jump-6462.mp3', audio => {
    jumpSoundRef.current = audio;
  });

  loadAudio(listener, 'Sound/crate-break-1-93926.mp3', audio => {
    breakSoundRef.current = audio;
  });

  loadAudio(listener, 'Sound/game-bonus-144751.mp3', audio => {
    winSoundRef.current = audio;
  });

  loadAudio(listener, 'Sound/french-horn-voice-fx-bad-joke-101443.mp3', audio => {
    gameOverSoundRef.current = audio;
  });
}