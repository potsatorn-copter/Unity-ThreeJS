import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Route, Routes } from 'react-router-dom';
import StartPage from './components/StartPage';
import CharacterSelection from './components/CharacterSelection';
import GamePage from './components/GamePage';
// คอมโพเนนต์หลักของแอปพลิเคชัน
const App = () => {
    return (_jsxs(Routes, { children: [_jsx(Route, { path: "/", element: _jsx(StartPage, {}) }), " ", _jsx(Route, { path: "/character-selection", element: _jsx(CharacterSelection, {}) }), " ", _jsx(Route, { path: "/game", element: _jsx(GamePage, {}) }), " "] }));
};
export default App;
