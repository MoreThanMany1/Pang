package Pang

import rl "vendor:raylib"

Paddle :: struct {
    rect: rl.Rectangle,
    speed: f32,
}

paddle_init :: proc(x, y, w, h, speed : f32) -> Paddle {
    return Paddle{rect = {x, y, w, h}, speed=speed}
}

paddle_move :: proc(p: ^Paddle, dt: f32) {
    dir: f32 = f32(int(rl.IsKeyDown(.S))) - f32(int(rl.IsKeyDown(.W)))
    p.rect.y += dir * p.speed * dt
}

paddle_render :: proc(p: Paddle) {
    rl.DrawRectangleRec(p.rect, rl.WHITE);
}