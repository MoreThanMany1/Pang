package Pang

import rl "vendor:raylib"

Ball :: struct {
    rect: rl.Rectangle,
    speed: f32,
}

ball_init :: proc(x, y, w, h, speed : f32) -> Ball {
    return Ball{rect = {x, y, w, h}, speed=speed}
}

ball_render :: proc(b: Ball) {
    rl.DrawRectangleRec(b.rect, rl.WHITE);
}