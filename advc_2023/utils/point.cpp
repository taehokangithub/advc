
#include <assert.h>
#include <sstream>
#include <vector>

#include "point.h"


using namespace std;

namespace advc_2023::utils
{
    int Point::s_log_dimemsion{ 2 };
    const vector<Point> Point::s_dir_8 =
    {
        { -1, -1 }, { 0, -1 }, { 1, -1 },
        { -1, 0 }, { 1, 0 },
        { -1, 1 }, { 0, 1 }, { 1, 1 }
    };

    const vector<Point> Point::s_dir_4 =
    {
        { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }
    };

    const vector<Dir> Point::s_dirs =
    {
        Dir::Up, Dir::Right, Dir::Down, Dir::Left
    };

    // -------------------------------------

    string Point::to_string() const
    {
        stringstream ss;
        ss << "[" << x << ":" << y;

        if (s_log_dimemsion >= 3)
        {
            ss << ":" << z;
        }
        if (s_log_dimemsion == 4)
        {
            ss << ":" << w;
        }
        ss << "]";

        assert(s_log_dimemsion <= 4);

        return ss.str();
    }

    // -------------------------------------

    string Point::to_unique_string() const
    {
        // As it takes to long using stringstream, now we use snprintf
        char buf[256];
        std::snprintf(buf, sizeof(buf), "[%d:%d:%d:%d]", x, y, z, w);
        return string(buf);
        /*
        stringstream ss;
        ss << "[" << x << ":" << y << ":" << z << ":" << w << "]";

        return ss.str(); */
    }

    // -------------------------------------

    Point Point::string_to_point(const string& str)
    {
        stringstream ss(str);
        Point p;
        char dummy;

        ss >> dummy >> p.x >> dummy >> p.y >> dummy >> p.z >> dummy >> p.w >> dummy;

        return p;
    }

    // -------------------------------------

    Dir Point::get_opposite_dir(Dir dir)
    {
        switch (dir)
        {
        case Dir::Down: return Dir::Up;
        case Dir::Right: return Dir::Left;
        case Dir::Up: return Dir::Down;
        case Dir::Left: return Dir::Right;
        }

        assert(false);
        return Dir::Up;
    }

    // -------------------------------------

    Point::Point(const Point& other)
    {
        x = other.x;
        y = other.y;
        z = other.z;
        w = other.w;
    }

    // -------------------------------------

    void Point::add(const Point& p)
    {
        x += p.x;
        y += p.y;
        z += p.z;
        w += p.w;
    }

    // -------------------------------------

    Point Point::get_added(const Point& p) const
    {
        Point ret(*this);
        ret.add(p);
        return ret;
    }

    // -------------------------------------

    bool Point::operator==(const Point& p) const
    {
        return x == p.x && y == p.y && z == p.z && w == p.w;
    }

    // -------------------------------------

    bool Point::operator<(const Point& p) const
    {
        if (x != p.x)
        {
            return x < p.x;
        }
        if (y != p.y)
        {
            return y < p.y;
        }
        if (z != p.z)
        {
            return z < p.z;
        }
        return w < p.w;
    }

    // -------------------------------------

    void Point::sub(const Point& p)
    {
        x -= p.x;
        y -= p.y;
        z -= p.z;
        w -= p.w;
    }

    // -------------------------------------

    Point Point::get_subbed(const Point& p) const
    {
        Point ret(*this);
        ret.sub(p);
        return ret;
    }

    // -------------------------------------

    void Point::rotate(Dir dir)
    {
        switch (dir)
        {
        case Dir::Up: 
            break;

        case Dir::Right: 
            std::swap(x, y);  
            x = -x;
            break;

        case Dir::Left: 
            std::swap(x, y); 
            y = -y;
            break;

        case Dir::Down: 
            x = -x; 
            y = -y; 
            break;

        default:
            assert(false);
        }
    }

    // -------------------------------------

    Point Point::get_rotated(Dir dir) const
    {
        Point ret(*this);
        ret.rotate(dir);
        return ret;
    }

    // -------------------------------------

    void Point::muliply(int val)
    {
        x *= val;
        y *= val;
        z *= val;
        w *= val;
    }

    // -------------------------------------

    Point Point::get_multipled(int val) const
    {
        Point ret(*this);
        ret.muliply(val);
        return ret;
    }

    // -------------------------------------

    void Point::divide(int val)
    {
        if (val == 0)
        {
            assert(false);
            return;
        }

        x /= val;
        y /= val;
        z /= val;
        w /= val;
    }

    // -------------------------------------

    Point Point::get_divided(int val) const
    {
        Point ret(*this);
        ret.divide(val);
        return ret;
    }

    // -------------------------------------

    void Point::move(Dir dir)
    {
        int dir_index = (int)dir;
        add(s_dir_4[dir_index]);
    }

    // -------------------------------------

    Point Point::get_moved(Dir dir) const
    {
        Point ret(*this);
        ret.move(dir);
        return ret;
    }

    // -------------------------------------

    void Point::set_max(const Point& p)
    {
        x = std::max(x, p.x);
        y = std::max(y, p.y);
        z = std::max(z, p.z);
        w = std::max(w, p.w);
    }

    // -------------------------------------

    void Point::set_min(const Point& p)
    {
        x = std::min(x, p.x);
        y = std::min(y, p.y);
        z = std::min(z, p.z);
        w = std::min(w, p.w);
    }

}