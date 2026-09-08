package Pang

import rl "vendor:raylib"

Ball :: struct {
    rect: rl.Rectangle,
    speed: f32,
    dir_vector: rl.Vector2,
}

ball_init :: proc(x, y, w, h, speed : f32) -> Ball {
    return Ball{rect = {x, y, w, h}, speed=speed, dir_vector=get_unit_vector_2_from_angle(get_pi_angle())}
}

ball_move :: proc(b: ^Ball, dt: f32) {
    b.rect.x += b.dir_vector.x * b.speed * dt
    b.rect.y += b.dir_vector.y * b.speed * dt
}

ball_follow :: proc(b: ^Ball) {
    m := rl.GetMousePosition();
    b.rect.x = m.x - b.rect.width/2;
    b.rect.y = m.y - b.rect.height/2;
}

ball_render :: proc(b: Ball) {
    rl.DrawRectangleRec(b.rect, rl.WHITE);
}