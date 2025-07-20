Fully dynamic system to write numbers with their units 
completely extensible

the core idea is that 
- everything is a [`Unit`](https://github.com/Galacticai/GalacticLib.Complete/blob/master/Math/Numerics/Numbers/Quantity/Units/Unit.cs) (abstract)
- unit has affix
  - affix has prefix+suffix
- units derive from `Unit` and set their rules
- including [`UnitSystem`](https://github.com/Galacticai/GalacticLib.Complete/blob/master/Math/Numerics/Numbers/Quantity/Units/UnitSystems/UnitSystem.cs)... it's also a `Unit` with custom setup to take factors (quecto.. kilo .. tera .. peta.. mebi .. kibi ....)

and much more

<img width="582" height="395" alt="image" src="https://github.com/user-attachments/assets/ebc3d9c6-55c6-4daa-bb40-53626a700e19" />
