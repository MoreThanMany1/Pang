package Pang

import rl "vendor:raylib"
//import "core:fmt"

main :: proc(){
    //Variables
    screen_width : i32 = 800;
    screen_height : i32 = 600;
    paddle_width : i32 = 20;
    paddle_height : i32 = 100;
    p1 := Paddle{20,20,paddle_width,paddle_height};
    p2 := Paddle{screen_width-20-paddle_width,screen_height-20-paddle_height,paddle_width,paddle_height};
    dt : f32;

    rl.InitWindow(screen_width, screen_height, "Pang");
    prev: time.Tick
    for !rl.WindowShouldClose() {
        dt := time.duration_seconds(time.tick_lap_time(&prev));
        rl.BeginDrawing();
        rl.ClearBackground(rl.BLACK);
        paddle_render(p1);
        paddle_render(p2);
        rl.EndDrawing()
    }
    rl.CloseWindow();
}