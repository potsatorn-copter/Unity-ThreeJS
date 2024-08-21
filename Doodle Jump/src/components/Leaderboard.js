import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
// Leaderboard.tsx
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Leaderboard.css';
import LeaderboardLocalStorage from 'core-utility/src/LeaderboardLocalStorage'; // ใช้ Local Storage
const Leaderboard = ({ latestScore, isWin, latestPlayerName, latestCharacter }) => {
    const navigate = useNavigate();
    const [leaderboard, setLeaderboard] = useState([]);
    const [highlightIndex, setHighlightIndex] = useState(null);
    const leaderboardHandler = new LeaderboardLocalStorage();
    // ฟังก์ชันสำหรับการนำทางไปยังหน้าเลือกตัวละคร
    const handleRestart = () => {
        navigate('/character-selection');
    };
    // ฟังก์ชันสำหรับการนำทางไปยังหน้าเริ่มต้น
    const handleBackToStart = () => {
        navigate('/');
    };
    // ฟังก์ชันสำหรับการรีเซ็ต leaderboard
    const handleResetLeaderboard = async () => {
        await leaderboardHandler.resetLeaderboard();
        const data = await leaderboardHandler.getLeaderboard();
        setLeaderboard(data);
    };
    // useEffect สำหรับการดึงข้อมูล leaderboard และการไฮไลท์สกอร์ล่าสุด
    useEffect(() => {
        const fetchData = async () => {
            // ดึงข้อมูล leaderboard จาก Local Storage
            const data = await leaderboardHandler.getLeaderboard();
            console.log('Fetched leaderboard data:', data);
            // เรียงลำดับข้อมูลจากมากไปน้อยและจัดกลุ่มชื่อที่เหมือนกัน ตัวละครที่เหมือนกัน และคะแนนที่เท่ากันให้อยู่ติดกัน
            const sortedData = data.sort((a, b) => {
                if (b.score !== a.score)
                    return b.score - a.score;
                if (b.name !== a.name)
                    return b.name.localeCompare(a.name);
                return b.character.localeCompare(a.character);
            });
            setLeaderboard(sortedData);
            // หาตำแหน่งของผู้เล่นที่มีชื่อ, คะแนน, และตัวละครล่าสุดในข้อมูล leaderboard 
            const latestIndex = sortedData.findLastIndex(player => player.score === latestScore && player.name === latestPlayerName && player.character === latestCharacter);
            setHighlightIndex(latestIndex);
        };
        fetchData();
    }, [latestScore, latestPlayerName, latestCharacter]);
    return (_jsx("div", { className: "leaderboard-overlay", children: _jsxs("div", { className: "leaderboard-container", children: [isWin ? (_jsx("div", { className: "win-message", children: "Congratulations! You Win!" })) : (_jsx("div", { className: "lose-message", children: "Game Over! Try Again!" })), _jsx("div", { id: "leaderboard", children: _jsx("ul", { id: "leaderboard-list", children: leaderboard.map((player, index) => (
                        // ไฮไลท์รายการที่มีข้อมูลตรงกับชื่อ, คะแนน, และตัวละครล่าสุด
                        _jsxs("li", { className: `leaderboard-entry ${index === highlightIndex ? 'highlight' : ''}`, children: [_jsxs("div", { className: "player-index", children: [index + 1, "."] }), _jsxs("div", { className: "player-info", children: [player.name, " "] }), _jsxs("div", { className: "player-character", children: [" - ", player.character, " "] }), _jsxs("div", { className: "player-score", children: [" - ", player.score, " "] })] }, index))) }) }), _jsx("button", { className: "btn btn-primary", onClick: handleRestart, children: "Back to Character Selection" }), _jsx("button", { className: "btn btn-secondary", onClick: handleBackToStart, children: "Back to Start" }), _jsx("button", { className: "btn btn-dark", onClick: handleResetLeaderboard, children: "Reset Leaderboard" })] }) }));
};
export default Leaderboard;
