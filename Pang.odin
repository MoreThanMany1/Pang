package Pang

import rl "vendor:raylib"
import "core:time"
//import "core:fmt"

SCREEN_WIDTH  :: 800
SCREEN_HEIGHT :: 600
PADDLE_WIDTH  :: 20
PADDLE_HEIGHT :: 100
PADDLE_SPEED  :: 200

BALL_SIZE :: 10
BALL_SPEED :: 250

main :: proc(){
    //Variables
    p1 := paddle_init(20,20,PADDLE_WIDTH,PADDLE_HEIGHT, PADDLE_SPEED, .W, .S);
    p2 := paddle_init(SCREEN_WIDTH-20.0-PADDLE_WIDTH,SCREEN_HEIGHT-20.0-PADDLE_HEIGHT,PADDLE_WIDTH,PADDLE_HEIGHT, PADDLE_SPEED, .UP, .DOWN);
    ball := ball_init((SCREEN_WIDTH-BALL_SIZE)/2,(SCREEN_HEIGHT-BALL_SIZE)/2,BALL_SIZE, BALL_SIZE, BALL_SPEED);

    rl.InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Pang");
    
    prev: time.Tick
    
    for !rl.WindowShouldClose() {
        
        dt := f32(time.duration_seconds(time.tick_lap_time(&prev)));
        
        rl.BeginDrawing();
        rl.ClearBackground(rl.BLACK);

        //Movement
        paddle_move(&p1, dt)
        paddle_move(&p2, dt)

        //Rendering
        paddle_render(p1);
        paddle_render(p2);
        ball_render(ball);

        rl.EndDrawing()
    }
    rl.CloseWindow();
}