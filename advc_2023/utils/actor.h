
#pragma once

#include <string>
#include <vector>

#include "point.h"

namespace advc_2023::utils
{
    struct Actor
    {
    public:
        Actor() {}

        Actor(const Point& p, const Dir d)
            : location(p), dir(d) {}

        Actor(const Actor& other); 

        void move();

        std::string to_string() const;

    public:
        Point location;
        Dir dir{};
    };
}