# BCPU
Bad CPU - Bad emulator for a Bad custom CPU

Based on the work of Ben Eater and Albert Paul Malvino (Digital computer electronics).

The BCPU is a 16 bit CPU with EEPROM based ROM and SRAM based RAM, EEPROM based control logic, LCD display, and more. This is a BCPU emulator written in C#, offering realtime execution. The emulator is to help work out some of the organizational kinks before building the entirety of the CPU (though some parts can be independently built). It will also serve to simplify the process of programming the microcode into the control logic EEPROMs, and to write and test programs that will run on the CPU.

### CPU Features
- 16 bit data bus, instructions
- A and B registers
- Conditional Jumps
- Hardware Stack
- 4 x 74LS181, 1 x 74LS182 ALU

### Emulator Features
- Emulates substructures of the cpu
- Variable real-time clock speed