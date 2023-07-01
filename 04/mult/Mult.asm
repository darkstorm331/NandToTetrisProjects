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

// First set the answer equal to the first number
@R0
D = M
@R2
M = D

// Take the second number and store it in the 3rd register. This is to track loops required
@R1
D = M
@R3
M = D

(LOOPSTART)
// Get number from 3rd register and decrement by 1
@R3
M = M-1
D = M

// If less than 0 then it means we're multiplying by zero and need to set result to 0
@ZERORESULT
D ; JLT

// If the number is 0 then we were multiplying by 1 so we can end here
@END
D ; JEQ

// Get register 0 value and add to result, then loop again
@R0
D = M
@R2
M = M+D
@LOOPSTART
0 ; JMP

// Multiplied by 0
(ZERORESULT)
@R2
M = 0

(END)
@END
0 ; JMP