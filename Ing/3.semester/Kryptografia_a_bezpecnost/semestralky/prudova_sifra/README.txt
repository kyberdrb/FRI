(Arch) Linux compilation and execution

Make sure you have "mono" installed:
  sudo pacman -S mono

Compilation
  msc Program.cs

Execution
  mono Program.exe

Profiling
  time mono Program.exe

Examples
  time mono Program.exe '/home/andrej/github/FRI/Ing/3.semester/Kryptografia_a_bezpecnost/semestralky/prudova_sifra/Texts/encText4.txt'

  time mono Program.exe '/home/andrej/github/FRI/Ing/3.semester/Kryptografia_a_bezpecnost/semestralky/prudova_sifra/Texts/encText3.txt' '/home/andrej/github/FRI/Ing/3.semester/Kryptografia_a_bezpecnost/semestralky/prudova_sifra/Texts/encText3.txt.dec'