package Pang

import rl "vendor:raylib"

Paddle :: struct {
    x, y: i32,
    width, height: i32,
}

paddle_render :: proc(p: Paddle) {
    rl.DrawRectangle(p.x, p.y, p.width, p.height, rl.WHITE);
}