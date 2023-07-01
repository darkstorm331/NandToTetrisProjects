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

(STARTLOOP)
// Store Memory Address of last screen Address in Register 15
@KBD
D = A
@R15
M = D

// Check for keyboard press and if nothing then jump to white
@KBD
D = M
@WHITE
D ; JEQ

// Set register 14 as -1 which will be black on screen
(BLACK)
@R14
M = -1
@DRAWSCREEN
0 ; JMP

// Set register 14 as 0 which will be white on screen
(WHITE)
@R14
M = 0
@DRAWSCREEN
0 ; JMP

// Start the draw screen loop
(DRAWSCREEN)

// Decrement the screen address by 1
@R15
AM = M-1

// Get the colour value from register 14
@R14
D = M

// Set the address back to the screen address and then set the colour
@R15
A = M
M = D

// Set the D register to the first screen address
@SCREEN
D = A

// Set A register back again
@R15
A = M

// Compare the current screen register against the first register to see if the screen is fully painted
D = D-A
@DRAWSCREEN
D ; JLT

// Endless Loop
@STARTLOOP
0 ; JMP