package Pang

import rl "vendor:raylib"
import "core:math/rand"
import "core:math"

get_pi_angle :: proc() -> f32 {
    return rand.float32_range(0,2*math.PI);
}

get_unit_vector_2_from_angle :: proc(angle: f32) -> rl.Vector2{
    return rl.Vector2{math.cos(angle), math.sin(angle)};
}