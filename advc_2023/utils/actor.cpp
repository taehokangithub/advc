
#pragma once

#include <sstream>

#include "actor.h"

using namespace std;

namespace advc_2023::utils
{

    Actor::Actor(const Actor& other)
    {
        location = other.location;
        dir = other.dir;
    }

    // -----------------------------------

    string Actor::to_string() const
    {
        stringstream ss;

        ss << "[" << location.to_string() << ":" << Point::get_dir_name(dir) << "]";

        return ss.str();
    }

    // -----------------------------------

    void Actor::move()
    {
        location.move(dir);
    }
}