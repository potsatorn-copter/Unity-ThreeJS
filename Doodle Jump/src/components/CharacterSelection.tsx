import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import DebugOverlay from './DebugOverlay';
import './CharacterSelection.css';

// นำเข้าภาพตัวละครจากไฟล์ต่างๆ
const AImage = new URL('../../public/3D_and_Picture/A-pic.png', import.meta.url).href;
const DImage = new URL('../../public/3D_and_Picture/D-pic.png', import.meta.url).href;
const DoodleImage = new URL('../../public/3D_and_Picture/doodle-removebg-preview.png', import.meta.url).href;

const CharacterSelection: React.FC = () => {
  const navigate = useNavigate(); // ใช้ hook useNavigate สำหรับการนำทาง
  const location = useLocation();
  const [playerName, setPlayerName] = useState<string | null>(location.state?.playerName || localStorage.getItem('playerName')); // สร้าง state สำหรับเก็บชื่อผู้เล่น
  const [selectedCharacter, setSelectedCharacter] = useState<string | null>(null); // สร้าง state สำหรับเก็บตัวละครที่ถูกเลือก

  useEffect(() => {
    console.log("Player Name: ", playerName); // ตรวจสอบค่า playerName
  }, [location.state, playerName]);

  // ฟังก์ชันสำหรับการเลือกตัวละคร
  const handleCharacterSelect = (character: string) => {
    setSelectedCharacter(character);
  };

  // ฟังก์ชันสำหรับการเริ่มเกม
  const handleStartGame = () => {
    if (selectedCharacter) {
      console.log('Navigating with character:', selectedCharacter);
      setTimeout(() => {
        navigate('/game', { state: { selectedCharacter, playerName } }); // นำทางไปยังหน้าของเกมพร้อมกับส่งข้อมูลตัวละครที่เลือกและชื่อผู้เล่น
      }, 1500); 
    }
  };

  // ข้อมูลสำหรับ debug overlay
  const debugInfo = {
    selectedCharacter,
    playerName,
  };

  // การแสดงผลของคอมโพเนนท์
  return (
    <div className="character-selection">
      <div className="character-title">Select Character</div>
      <div className="character-container">
        <img
          src={AImage}
          alt="Character A"
          className={`character-image ${selectedCharacter === 'A' ? 'selected' : ''}`} // เพิ่มคลาส 'selected' หากตัวละคร A ถูกเลือก
          onClick={() => handleCharacterSelect('A')} // กำหนดฟังก์ชันสำหรับการเลือกตัวละคร A
        />
        <img
          src={DImage}
          alt="Character D"
          className={`character-image ${selectedCharacter === 'D' ? 'selected' : ''}`} // เพิ่มคลาส 'selected' หากตัวละคร D ถูกเลือก
          onClick={() => handleCharacterSelect('D')} // กำหนดฟังก์ชันสำหรับการเลือกตัวละคร D
        />
        <img
          src={DoodleImage}
          alt="Character Doodle"
          className={`character-image ${selectedCharacter === 'Doodle' ? 'selected' : ''}`} // เพิ่มคลาส 'selected' หากตัวละคร Doodle ถูกเลือก
          onClick={() => handleCharacterSelect('Doodle')} // กำหนดฟังก์ชันสำหรับการเลือกตัวละคร Doodle
        />
      </div>
      <button className="btn-custom" onClick={handleStartGame} disabled={!selectedCharacter}>
        Select Character
      </button>
      <DebugOverlay debugInfo={debugInfo} /> {/* แสดงข้อมูล debug */}
    </div>
  );
};

export default CharacterSelection;