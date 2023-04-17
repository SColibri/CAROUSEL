#pragma once

// c++
#include <string>
#include <vector>

// catch2
#include <catch2/catch_test_macros.hpp>

// AMModels
#include "../../../AMModels/include/modelSchema.h"

// Matcalc api
#include "../../../AM_API_lib/matcalc/include/TextExtractor/PhaseNamesExtractor.h"

TEST_CASE("Extractors")
{
	/// <summary>
	/// Phase names extractor
	/// </summary>
	SECTION("Extract phases using string")
	{
		// this is an example of a string containing duplicated phases with # identifiers
		// and also scheil variable definitions. We are interested on extracting only the phases
		// that are used and that might not be contained in the database as for example
		// the generated phases with '#' char.
		std::string textToParse = " This is some previous text \n phases:\n\
			LIQUID                    FCC_A1                    FCC_A1#01\n\
			FCC_A1#02                 BCC_A2                    HCP_A3\n\
			HCP_A3#01                 HCP_A3#02                 DELTA\n\
			GAMMA_DP                  GAMMA_PRIME               LAV_C14\n\
			LAVES                     MU_PHASE                  NIAL\n\
			NIAL#01                   NI2CR                     NITI2\n\
			SIGMA                     M6C                       M23C6\n\
			FCC_A1_S                  FCC_A1#01_S               FCC_A1#02_S\n\
			BCC_A2_S                  HCP_A3_S                  HCP_A3#01_S\n\
			HCP_A3#02_S               DELTA_S                   GAMMA_DP_S\n\
			GAMMA_PRIME_S             LAV_C14_S                 LAVES_S\n\
			MU_PHASE_S                NIAL_S                    NIAL#01_S\n\
			NI2CR_S                   NITI2_S                   SIGMA_S\n\
			M6C_S                     M23C6_S";

		// Object to test
		APIMatcalc::Extractors::PhaseNameExtractor extractor;

		// vector output, this should only contain the phase names
		std::vector<std::string> result = extractor.extract(textToParse);
		REQUIRE(result.size() == 41);
		
		bool gi = false;
	}
}