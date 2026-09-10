### IsSuitableFor
**huidig gedrag**: Er hoeft maar 1 skill te matchen tussen course en coach in de plaats van alle.

**Gewenst gedrag**:Alle skills van course moeten minstens in de skills van coach aanezig zijn

**Test**: Coach_Is_Not_Suitable_When_Skills_Doesnt_match

**Opgelost?**: ja

### UpdateSkills
**Huidig gedrag**: Bij het updaten van de skills van coach wordt de skill-list eerst geleegd en daarna pas gecheckt of de skills ok zijn. Dit zorgt ervoor dat we geen skills meer hebben in dat geval

**Gewenst gedrag**: Skill list wordt niet gecleared indien de nieuwe skills niet aanvaard worden

**Test**: Coach_Can_Update_Skills_Atomic

**Opgelost**: Ja

