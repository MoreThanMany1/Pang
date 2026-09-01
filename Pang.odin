package Pang

import rl "vendor:raylib"
import "core:time"
//import "core:fmt"

main :: proc(){
    //Variables
    screen_width : i32 = 800;
    screen_height : i32 = 600;
    paddle_width : f32 = 20;
    paddle_height : f32 = 100;
    paddle_speed : f32 = 200;
    p1 := paddle_init(20,20,paddle_width,paddle_height, paddle_speed);
    p2 := paddle_init(f32(screen_width)-20.0-paddle_width,f32(screen_height)-20.0-paddle_height,paddle_width,paddle_height, paddle_speed);
    dt : f32;

    rl.InitWindow(screen_width, screen_height, "Pang");
    
    prev: time.Tick
    
    for !rl.WindowShouldClose() {
        
        dt := f32(time.duration_seconds(time.tick_lap_time(&prev)));
        
        rl.BeginDrawing();
        rl.ClearBackground(rl.BLACK);

        //Movement
        paddle_move(&p1, dt)

        //Rendering
        paddle_render(p1);
        paddle_render(p2);

        rl.EndDrawing()
    }
    rl.CloseWindow();
}