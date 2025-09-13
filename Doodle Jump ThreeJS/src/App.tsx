import React from 'react';
import { Route, Routes } from 'react-router-dom';
import StartPage from './components/StartPage';
import CharacterSelection from './components/CharacterSelection';
import GamePage from './components/GamePage';

// คอมโพเนนต์หลักของแอปพลิเคชัน
const App: React.FC = () => {
  return (
    <Routes>
      {/* กำหนดเส้นทางของแต่ละหน้า */}
      <Route path="/" element={<StartPage />} /> {/* เส้นทางสำหรับหน้าเริ่มต้น */}
      <Route path="/character-selection" element={<CharacterSelection />} /> {/* เส้นทางสำหรับหน้าเลือกตัวละคร */}
      <Route path="/game" element={<GamePage />} /> {/* เส้นทางสำหรับหน้าเกม */}
    </Routes>
  );
};

export default App;