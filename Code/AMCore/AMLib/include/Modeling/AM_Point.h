#pragma once

namespace modeling
{
    struct AM_Point {
        float x;
        float y;
        float z;

        AM_Point() : x(0), y(0), z(0) {}
        AM_Point(float xp, float yp, float zp) : x(xp), y(yp), z(zp) {}
    };
}