import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import DebugOverlay from './DebugOverlay';
import './CharacterSelection.css';
// นำเข้าภาพตัวละครจากไฟล์ต่างๆ
const AImage = new URL('../../public/3D_and_Picture/A-pic.png', import.meta.url).href;
const DImage = new URL('../../public/3D_and_Picture/D-pic.png', import.meta.url).href;
const DoodleImage = new URL('../../public/3D_and_Picture/doodle-removebg-preview.png', import.meta.url).href;
const CharacterSelection = () => {
    const navigate = useNavigate(); // ใช้ hook useNavigate สำหรับการนำทาง
    const location = useLocation();
    const [playerName, setPlayerName] = useState(location.state?.playerName || localStorage.getItem('playerName')); // สร้าง state สำหรับเก็บชื่อผู้เล่น
    const [selectedCharacter, setSelectedCharacter] = useState(null); // สร้าง state สำหรับเก็บตัวละครที่ถูกเลือก
    useEffect(() => {
        console.log("Player Name: ", playerName); // ตรวจสอบค่า playerName
    }, [location.state, playerName]);
    // ฟังก์ชันสำหรับการเลือกตัวละคร
    const handleCharacterSelect = (character) => {
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
    return (_jsxs("div", { className: "character-selection", children: [_jsx("div", { className: "character-title", children: "Select Character" }), _jsxs("div", { className: "character-container", children: [_jsx("img", { src: AImage, alt: "Character A", className: `character-image ${selectedCharacter === 'A' ? 'selected' : ''}`, onClick: () => handleCharacterSelect('A') }), _jsx("img", { src: DImage, alt: "Character D", className: `character-image ${selectedCharacter === 'D' ? 'selected' : ''}`, onClick: () => handleCharacterSelect('D') }), _jsx("img", { src: DoodleImage, alt: "Character Doodle", className: `character-image ${selectedCharacter === 'Doodle' ? 'selected' : ''}`, onClick: () => handleCharacterSelect('Doodle') })] }), _jsx("button", { className: "btn-custom", onClick: handleStartGame, disabled: !selectedCharacter, children: "Select Character" }), _jsx(DebugOverlay, { debugInfo: debugInfo }), " "] }));
};
export default CharacterSelection;
