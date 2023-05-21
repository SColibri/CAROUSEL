
-----------------------------------------------------------------------------------------------------------------------
--												HEAT TREATMENT
-----------------------------------------------------------------------------------------------------------------------

-- view that sorts in descending order the simulation data
database_create_view("CREATE VIEW IF NOT EXISTS v_grouped_heat_treatment_simulation AS "..
							"SELECT * FROM PrecipitateSimulationData "..
							"ORDER BY IDHeatTreatment, ID DESC;")

-- view that contains last values of the simulation set
database_create_view("CREATE VIEW IF NOT EXISTS v_last_heat_treatment_simulation AS "..
						    "SELECT * FROM v_grouped_heat_treatment_simulation "..
							"GROUP BY IDHeatTreatment, IDPrecipitationPhase;")

-- view that contains all data necessary for the project overview plotting
database_create_view("CREATE VIEW IF NOT EXISTS vd_HeatTreatment_Plot AS "..
      "SELECT v_last_heat_treatment_simulation.*,"..
        "[Case].IDProject,"..
        "HeatTreatment.IDCase,"..
        "HeatTreatment.Name AS HeatTreatment,"..
        "PrecipitationDomain.Name AS PrecipitationDomain,"..
        "PrecipitationPhase.Name AS PrecipitationPhase "..
      "FROM v_last_heat_treatment_simulation "..
      "INNER JOIN "..
      "HeatTreatment ON HeatTreatment.ID = v_last_heat_treatment_simulation.IDHeatTreatment "..
      "INNER JOIN "..
      "PrecipitationDomain ON PrecipitationDomain.ID = HeatTreatment.IDPrecipitationDomain "..
      "INNER JOIN "..
      "PrecipitationPhase ON PrecipitationPhase.ID = v_last_heat_treatment_simulation.IDPrecipitationPhase "..
      "INNER JOIN "..
      "[Case] ON [Case].ID = HeatTreatment.IDCase;")

-----------------------------------------------------------------------------------------------------------------------
--												SELECTED ELEMENTS
-----------------------------------------------------------------------------------------------------------------------

database_create_view("CREATE VIEW IF NOT EXISTS vd_SelectedElements AS "..
    "SELECT "..
    "SelectedElements.IDProject, "..
    "SelectedElements.isReferenceElement, "..
    "Element.Name AS Element "..
    "FROM SelectedElements "..
    "INNER JOIN "..
    "Element ON Element.ID = SelectedElements.IDElement")

-----------------------------------------------------------------------------------------------------------------------
--												ELEMENT COMPOSITION
-----------------------------------------------------------------------------------------------------------------------

database_create_view("CREATE VIEW IF NOT EXISTS vd_ElementComposition AS "..
"SELECT "..
"ElementComposition.IDCase, "..
"Element.Name As Element, "..
"ElementComposition.Value "..
"FROM ElementComposition "..
"INNER JOIN "..
"Element ON Element.ID = ElementComposition.IDElement")

database_create_view("CREATE VIEW IF NOT EXISTS vd_case_composition AS "..
    "SELECT "..
    "[Case].ID,"..
    "ElementComposition.Value,"..
    "Element.Name "..
    "FROM `Case` "..
    "INNER JOIN "..
    "ElementComposition ON ElementComposition.IDCase = [Case].ID "..
    "INNER JOIN "..
    "Element ON Element.ID = ElementComposition.IDElement;")

-----------------------------------------------------------------------------------------------------------------------
--												SCHEIL SIMULATIONS
-----------------------------------------------------------------------------------------------------------------------

database_create_view("CREATE VIEW IF NOT EXISTS vd_ScheilSimulation AS "..
    "Select DISTINCT ScheilPhaseFraction.Temperature, ScheilPhaseFraction.Value as phaseFraction, Phase.Name as phaseName, Projects.Name as projectName, ScheilPhaseFraction.IDCase, ScheilPhaseFraction.IDPhase, [Case].IDProject FROM ScheilPhaseFraction "..
    "INNER JOIN "..
    "[Case] ON [Case].ID = ScheilPhaseFraction.IDCase "..
    "INNER JOIN "..
    "Phase ON Phase.ID = ScheilPhaseFraction.IDPhase "..
    "INNER JOIN "..
    "Projects ON Projects.ID = [Case].IDProject "..
    "WHERE phaseFraction > 0 "..
    "ORDER BY ScheilPhaseFraction.IDCase, ScheilPhaseFraction.IDPhase, ScheilPhaseFraction.Temperature;")

-- Creates view that groups phases and cases by solidification temperature (lowest temp available)
database_create_view("DROP VIEW IF EXISTS v_scheil_solidification; "..
    "CREATE VIEW IF NOT EXISTS v_scheil_solidification AS "..
    "SELECT phaseFraction, phaseName, IDCase, IDPhase, IDProject, MIN(Temperature) AS solidification_temperature "..
    "FROM vd_ScheilSimulation "..
    "GROUP BY IDCase, phaseName; ")

-----------------------------------------------------------------------------------------------------------------------
--												EQUILIBRIUM SIMULATIONS
-----------------------------------------------------------------------------------------------------------------------

database_create_view("Select DISTINCT EquilibriumPhaseFraction.Temperature, EquilibriumPhaseFraction.Value as phaseFraction, Phase.Name as phaseName, Projects.Name as projectName, EquilibriumPhaseFraction.IDCase, EquilibriumPhaseFraction.IDPhase, [Case].IDProject FROM EquilibriumPhaseFraction "..
"INNER JOIN "..
"[Case] ON [Case].ID = EquilibriumPhaseFraction.IDCase "..
"INNER JOIN "..
"Phase ON Phase.ID = EquilibriumPhaseFraction.IDPhase "..
"INNER JOIN "..
"Projects ON Projects.ID = [Case].IDProject "..
"WHERE phaseFraction > 0 "..
"ORDER BY EquilibriumPhaseFraction.IDCase, EquilibriumPhaseFraction.IDPhase, EquilibriumPhaseFraction.Temperature;")