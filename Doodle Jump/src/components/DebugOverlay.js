import { jsxs as _jsxs, jsx as _jsx } from "react/jsx-runtime";
// คอมโพเนนต์ DebugOverlay สำหรับแสดงข้อมูล debug บนหน้าจอ
const DebugOverlay = ({ debugInfo }) => {
    return (_jsx("div", { style: {
            position: 'absolute',
            top: '100px',
            left: '10px',
            backgroundColor: 'rgba(0,0,0,0.7)',
            color: 'white',
            padding: '10px',
            borderRadius: '5px',
            zIndex: 1000
        }, children: Object.keys(debugInfo).map((key) => (_jsxs("div", { children: [_jsxs("strong", { children: [key, ":"] }), " ", JSON.stringify(debugInfo[key])] }, key))) }));
};
export default DebugOverlay;
