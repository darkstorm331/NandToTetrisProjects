// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.

// Put your code here.



(LOOP)
    //Set 8000 loops for screen space
    @8192
    D=A
    @loops
    M=D

    @colour
    M=0

    @i
    M=0

    @currentpixel
    M=0

    @KBD
    D=M

    //Set colour to black or white
    @BLACK
    D;JNE
    @WHITE
    D;JEQ

    (BLACK)
        @colour
        M=-1
        @PAINTSCRN
        0;JMP

    (WHITE)
        @colour
        M=0
        @PAINTSCRN
        0;JMP

    (PAINTSCRN)
        @i
        D=M
        @loops
        D=D-M
        @LOOP
        D;JEQ

        @i
        D=M
        @SCREEN    
        A=A+D
        D=A
        @currentpixel
        M=D

        @colour
        D=M
        @currentpixel
        A=M

        M=D

        @i
        M=M+1
    @PAINTSCRN
    0;JMP
@LOOP
0;JMP