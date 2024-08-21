import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import LeaderboardLocalStorage from 'core-utility/src/LeaderboardLocalStorage';
const StartPage = () => {
    const navigate = useNavigate();
    const [playerName, setPlayerName] = useState('');
    const [error, setError] = useState('');
    const leaderboardHandler = new LeaderboardLocalStorage();
    useEffect(() => {
        setError(''); // เคลียร์ error ทุกครั้งที่ playerName เปลี่ยน
    }, [playerName]);
    const handleStartClick = async () => {
        if (playerName.trim() === '') {
            setError('Please enter a valid name.');
        }
        else if (await leaderboardHandler.isNameDuplicate(playerName)) { // เช็คชื่อที่ซ้ำกันจาก Local Storage
            setError('This name is already taken. Please choose a different name.');
        }
        else {
            localStorage.setItem('playerName', playerName); // บันทึกชื่อใน Local Storage
            navigate('/character-selection', { state: { playerName } });
        }
    };
    return (_jsxs("div", { className: "start-page", children: [_jsx("div", { className: "title-container", children: _jsx("h1", { className: "title", children: "DooDleJump 3D" }) }), _jsxs("div", { className: "input-container", children: [_jsx("input", { type: "text", placeholder: "Enter your name", value: playerName, onChange: (e) => setPlayerName(e.target.value), maxLength: 10 }), error && _jsx("p", { className: "error", children: error })] }), _jsx("div", { className: "button-container text-center", children: _jsxs("button", { className: "btn-custom", onClick: handleStartClick, children: [_jsx("i", { className: "fas fa-play" }), " Start Game"] }) })] }));
};
export default StartPage;
