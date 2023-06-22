// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Mult.asm

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)
//
// This program only needs to handle arguments that satisfy
// R0 >= 0, R1 >= 0, and R0*R1 < 32768.

// Put your code here.

//Take first number
@R0
D=M
@num1
M=D

//Take second number
@R1
D=M
@num2
M=D

//iterations
@i
M=0

//Store result
@R2
M=0

//Start Loop
(LOOP)
    @i
    D=M
    @num2
    D=D-M
    @END
    D;JEQ

    @R2
    D=M
    @num1
    D=D+M
    @R2
    M=D

    @i
    M=M+1

    @LOOP
    0;JMP

(END)
    @END
    0;JMP