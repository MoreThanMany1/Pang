package Pang

import rl "vendor:raylib"

Paddle :: struct {
    rect: rl.Rectangle,
    speed: f32,
    up, down: rl.KeyboardKey,
}

paddle_init :: proc(x, y, w, h, speed : f32, up, down: rl.KeyboardKey) -> Paddle {
    return Paddle{rect = {x, y, w, h}, speed=speed, up=up, down=down}
}

paddle_move :: proc(p: ^Paddle, dt: f32) {
    dir: f32 = f32(int(rl.IsKeyDown(p.down))) - f32(int(rl.IsKeyDown(p.up)))
    p.rect.y += dir * p.speed * dt
    p.rect.y = clamp(p.rect.y, 0, SCREEN_HEIGHT - p.rect.height)
}

paddle_render :: proc(p: Paddle) {
    rl.DrawRectangleRec(p.rect, rl.WHITE);
}