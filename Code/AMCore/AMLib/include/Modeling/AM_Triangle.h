#pragma once

#include "AM_Point.h"

namespace modeling
{
    struct AM_Triangle {
        AM_Point normal;
        AM_Point v1;
        AM_Point v2;
        AM_Point v3;
        AM_Triangle(AM_Point normalp, AM_Point v1p, AM_Point v2p, AM_Point v3p) :
            normal(normalp), v1(v1p), v2(v2p), v3(v3p) {}
    };
}