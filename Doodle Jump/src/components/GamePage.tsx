import React, { useEffect, useState, useRef, useCallback } from 'react';
import * as THREE from 'three';
import { AudioListener, PositionalAudio } from 'three';
import { useLocation, useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

import { loadCharacter } from './CharacterSetup'; // โหลดโมดูลสำหรับตั้งค่าตัวละคร
import { createControls } from './Controls'; // สร้างตัวควบคุมสำหรับกล้อง
import { setupKeyControls, cleanupKeyControls } from './KeyControls'; // ตั้งค่าการควบคุมแป้นพิมพ์
import { createScoreDisplay, updateScoreDisplay, removeScoreDisplay } from './ScoreDisplay'; // จัดการการแสดงผลคะแนน
import DebugOverlay from './DebugOverlay'; // คอมโพเนนท์สำหรับแสดงข้อมูล debug
import Leaderboard from './Leaderboard'; // คอมโพเนนท์สำหรับแสดง leaderboard
import { setupEventHandlers } from './EventHandlers'; // ตั้งค่า event handlers
import { initializeScene, initializeCamera, initializeRenderer } from './SceneSetup'; // ตั้งค่าฉาก, กล้อง และ renderer
import { createPlatform, resetPlatforms, recyclePlatforms, createBluePlatformNearby, createInitialPlatforms 
  , checkPlatformCollision, createNewPlatform} from './PlatformManager'; // จัดการ platform

import { setupLights } from 'core-utility/src/LightSetup'; // ตั้งค่าแสง
import { loadSounds } from 'core-utility/src/Audio'; // โหลดเสียง
import LeaderboardLocalStorage from 'core-utility/src/LeaderboardLocalStorage'; // เปลี่ยนเป็นใช้ Local Storage
import { LeaderboardData } from 'core-utility/src/types'; // ค่าใน Leaderboard

const GamePage: React.FC = () => {
  const location = useLocation();
  const { selectedCharacter, playerName } = location.state || { selectedCharacter: 'Doodle', playerName: localStorage.getItem('playerName') || '' };
  const leaderboardHandler = new LeaderboardLocalStorage();
  const [score, setScore] = useState(0); // state สำหรับคะแนน
  const [debugInfo, setDebugInfo] = useState({}); // state สำหรับ debug information
  const [isGameOver, setIsGameOver] = useState(false); // state สำหรับสถานะเกมจบ
  const [isWin, setIsWin] = useState(false); // state สำหรับสถานะชนะเกม
  const [timeLeft, setTimeLeft] = useState(90); // state สำหรับเวลาที่เหลือ
  const [latestScore, setLatestScore] = useState(0); // state สำหรับคะแนนล่าสุด
  const [leaderboard, setLeaderboard] = useState<LeaderboardData[]>([]); // state สำหรับ leaderboard

  const scoreRef = useRef(0); // ตัวแปร ref สำหรับคะแนน
  const timerRef = useRef<NodeJS.Timeout | null>(null); // ตัวแปร ref สำหรับ timer
  const keys = useRef<{ [key: string]: { pressed: boolean } }>({
    a: { pressed: false },
    d: { pressed: false }
  }).current;

  const jumpSoundRef = useRef<PositionalAudio>(); // ตัวแปร ref สำหรับเสียงกระโดด
  const breakSoundRef = useRef<PositionalAudio>(); // ตัวแปร ref สำหรับเสียงแพลตฟอร์มแตก
  const winSoundRef = useRef<PositionalAudio>(); // ตัวแปร ref สำหรับเสียงชนะ
  const gameOverSoundRef = useRef<PositionalAudio>(); // ตัวแปร ref สำหรับเสียงจบเกม

  const characterRef = useRef<THREE.Group | null>(null); // ตัวแปร ref สำหรับตัวละคร
  const hasGameEnded = useRef(false); // ตัวแปร ref สำหรับสถานะเกมจบ

  const updateDebugInfo = (newInfo: any) => {
    setDebugInfo(prevDebugInfo => ({ ...prevDebugInfo, ...newInfo }));
  };

  const navigate = useNavigate();

  let scene: THREE.Scene; // ตัวแปรสำหรับ scene
  let camera: THREE.PerspectiveCamera; // ตัวแปรสำหรับ camera
  let renderer: THREE.WebGLRenderer; // ตัวแปรสำหรับ renderer
  let isRunning: boolean; // ตัวแปรสำหรับสถานะการทำงานของเกม
  const isJumping = useRef(false); // ตัวแปร ref สำหรับสถานะการกระโดด
  let highestPlatformY: number; // ตัวแปรสำหรับตำแหน่ง Y ของแพลตฟอร์มที่สูงที่สุด
  const minHeightRef = useRef(0); // ตัวแปร ref สำหรับความสูงขั้นต่ำ
  const maxGap = 4; // ช่องว่างสูงสุดระหว่างแพลตฟอร์ม
  const winScore = 430; // คะแนนที่ต้องการเพื่อชนะเกม
  const jumpVelocity = 0.3; // ความเร็วในการกระโดด
  const gravity = -0.002; // แรงโน้มถ่วง
  let platforms: THREE.Mesh[] = []; // ตัวแปรสำหรับเก็บแพลตฟอร์ม
  let redPlatformCount = 0; // ตัวแปรสำหรับนับจำนวนแพลตฟอร์มสีแดง

  // ตำแหน่งเริ่มต้นของแพลตฟอร์ม
  const platformPositions = [
    { x: 0, y: 5, z: 0, color: '#0369a1' },
    { x: 0, y: 10, z: 10, color: '#0369a1' },
    { x: 0, y: 15, z: -7, color: '#ff0000' },
    { x: 0, y: 20, z: 7, color: '#0369a1' },
    { x: 0, y: 25, z: -10, color: '#ff0000' },
    { x: 0, y: 30, z: 10, color: '#0369a1' },
    { x: 0, y: 35, z: -7, color: '#ff0000' },
    { x: 0, y: 40, z: 7, color: '#0369a1' }
  ];

  // ฟังก์ชันสำหรับรีเซ็ตเกม
  function resetGame() {
    console.log('Resetting game...');
    hasGameEnded.current = false;
    if (characterRef.current) {
      characterRef.current.position.set(0, 6, 0); // ตั้งค่าตำแหน่งของตัวละคร
      (characterRef.current as any).velocity = { x: 0, y: jumpVelocity, z: 0 };
    }
    ({ highestPlatformY, minHeight: minHeightRef.current } = resetPlatforms(scene, platforms, platformPositions, highestPlatformY, minHeightRef.current)); // รีเซ็ตแพลตฟอร์ม
    scoreRef.current = 0;
    setScore(0);
    setTimeLeft(120);
    updateScoreDisplay(0);
    console.log('Game reset complete');
    setIsGameOver(false);
    setIsWin(false);
  }

  // ฟังก์ชันสำหรับเริ่ม timer
  function startTimer() {
    timerRef.current = setInterval(() => {
      setTimeLeft(prevTime => {
        if (prevTime <= 1) {
          clearInterval(timerRef.current!);
          isRunning = false;

          setLatestScore(scoreRef.current);

          if (!hasGameEnded.current) {
            if (scoreRef.current >= winScore) {
              setIsWin(true);
              winSoundRef.current?.play();
            } else {
              setIsGameOver(true);
              gameOverSoundRef.current?.play();
            }

            updateDebugInfo({
              latestScore: scoreRef.current,
            });

            endGame(); 
          }
        }
        return prevTime - 1;
      });
    }, 1000);
  }

  // ฟังก์ชันสำหรับจบเกม
  const endGame = useCallback(async () => { 
    if (!hasGameEnded.current) {
      hasGameEnded.current = true;
      setLatestScore(scoreRef.current);
      if (scoreRef.current >= winScore) {
        setIsWin(true);
        winSoundRef.current?.play();
      } else {
        setIsGameOver(true);
        gameOverSoundRef.current?.play();
      }

      updateDebugInfo({
        latestScore: scoreRef.current,
      });

      // อัปเดตข้อมูล Leaderboard 
      await leaderboardHandler.updateLeaderboard(playerName, scoreRef.current, selectedCharacter); 
      const leaderboardData = await leaderboardHandler.getLeaderboard();
      setLeaderboard(leaderboardData);
      console.log('Updated leaderboard data:', leaderboardData);
    }
  }, [winScore, playerName]);

  // ฟังก์ชันอนิเมท
  function animate() {
    if (!isRunning) return;

    requestAnimationFrame(animate);
    renderer.render(scene, camera);

    updateDebugInfo({
      characterPosition: characterRef.current?.position,
      velocity: (characterRef.current as any)?.velocity,
      score: scoreRef.current,
      minHeight: minHeightRef.current,
      winScore
    });

    //เคลื่อนที่ซ้ายขวา
    if (characterRef.current) {
      (characterRef.current as any).velocity = (characterRef.current as any).velocity || { x: 0, y: -0.01, z: 0 };
      characterRef.current.position.z += keys.d.pressed ? -0.07 : keys.a.pressed ? 0.07 : 0;

      if (isJumping.current) {
        (characterRef.current as any).velocity.y += gravity;
        characterRef.current.position.y += (characterRef.current as any).velocity.y;

        // เช็คการชนของตัวละครกับแพลตฟอร์ม
        checkPlatformCollision(characterRef, platforms, scene, highestPlatformY, maxGap, minHeightRef, isJumping, scoreRef,jumpVelocity,
          gravity,
          jumpSoundRef,
          breakSoundRef,
          updateDebugInfo,
          endGame,
          createNewPlatform
        );
      } else {
        (characterRef.current as any).velocity.y = jumpVelocity;
        isJumping.current = true;
      }
      
      camera.position.y = characterRef.current.position.y + 2; // ปรับตำแหน่งกล้องให้ตามตัวละคร

      if (characterRef.current.position.y <= minHeightRef.current - 1) {
        isRunning = false;
        isJumping.current = false;
        endGame();
      }
      
      highestPlatformY = recyclePlatforms(scene, camera, platforms, highestPlatformY, maxGap, createBluePlatformNearby);
      
      if (characterRef.current && characterRef.current.position.y > scoreRef.current) {
        scoreRef.current = Math.floor(characterRef.current.position.y);
        setScore(scoreRef.current);
        updateScoreDisplay(scoreRef.current);
      }
    }
  }

  useEffect(() => {
    console.log('useEffect triggered', selectedCharacter, playerName);

    scene = initializeScene(); // สร้างและตั้งค่า scene
    camera = initializeCamera(); // สร้างและตั้งค่า camera
    renderer = initializeRenderer(); // สร้างและตั้งค่า renderer

    const cleanupResizeHandler = setupEventHandlers(camera, renderer);
    const controls = createControls(camera, renderer); // สร้าง controls สำหรับ camera

    const listener = new AudioListener();
    camera.add(listener);

    loadSounds(listener, jumpSoundRef, breakSoundRef, winSoundRef, gameOverSoundRef); // โหลดเสียงต่างๆ

    const sounds = {
      jumpSound: jumpSoundRef.current!,
      breakSound: breakSoundRef.current!,
      winSound: winSoundRef.current!,
      gameOverSound: gameOverSoundRef.current!
    };

    // โหลดตัวละคร
    loadCharacter(scene, sounds, character => {
      characterRef.current = character;
      console.log('Character loaded', character);
    }, selectedCharacter);

    // สร้างแพลตฟอร์มเริ่มต้น
    ({ highestPlatformY, minHeight: minHeightRef.current } = createInitialPlatforms(platformPositions, scene, platforms, highestPlatformY, minHeightRef.current, redPlatformCount, createBluePlatformNearby));
    
     setupLights(scene, new THREE.Vector3(15, 15, 15));

    const { handleKeyDown, handleKeyUp } = setupKeyControls(keys, characterRef); // ตั้งค่าการควบคุมคีย์บอร์ด

    isRunning = true;
    startTimer();
    animate(); // เริ่มการอนิเมท

    return () => {
      if (timerRef.current) {
        clearInterval(timerRef.current);
      }
      cleanupResizeHandler();
      cleanupKeyControls(handleKeyDown, handleKeyUp);
      document.body.removeChild(renderer.domElement);
      removeScoreDisplay();
    };
  }, [selectedCharacter, playerName]);

  return (
    <div>
      {isWin || isGameOver ? (
        <Leaderboard latestScore={latestScore} isWin={isWin} latestPlayerName={playerName} latestCharacter={selectedCharacter}/>
      ) : (
        <>
          <div id="button-container" className="d-none text-center">
          </div>
          <div id="message-container" className="message-container text-center">
            <div id="message" className="message"></div>
            <button id="restartButton" className="btn btn-primary btn-lg m-2 d-none">Restart Game</button>
          </div>
          <div className="timer">
            Time left: {timeLeft} seconds
          </div>
          <DebugOverlay debugInfo={debugInfo} />
        </>
      )}
    </div>
  )
};

export default GamePage;