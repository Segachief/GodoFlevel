# GodoFlevel
Flevel component for Godo, based on 7h program by Iros

This is a prototype extension for the FF7 Randomisation & Game Adjustment tool, Godo. Currently it works as its own stand-alone tool
and will be integrated into the main tool when both are more complete/stable. It is based on the original tool made by Iros for his
modding framework, 7th Heaven, and uses the algorithms for encoding/decoding an flevel found in that project. All code within this
project, and the project's licence, conforms to the licence that the 7h tool was supplied under.

GodoFlevel takes an flevel, chunks it, and then edits the individual chunks where appropriate before rebuilding it into a usable
flevel. It can also do the same for PSX field files, though some options are unavailable due to differences between these fields
and the PC version of the fields.

Current Features 
-) Model + Animation swapping (PC only)
-) Item/Materia placement
