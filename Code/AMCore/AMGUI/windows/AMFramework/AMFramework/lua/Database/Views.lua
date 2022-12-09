
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
