
#pragma once

#include <string>
#include <vector>

namespace advc_2023::utils
{
    enum class Dir { Up, Right, Down, Left };
    
    class Point
    {
    public:
        Point() {}
        Point(int _x, int _y) : x(_x), y(_y) {}
        Point(int _x, int _y, int _z) : x(_x), y(_y), z(_z) {}
        Point(int _x, int _y, int _z, int _w) : x(_x), y(_y), z(_z), w(_w) {}
        Point(const Point& other);
        bool operator==(const Point& p) const;

        void add(const Point& p);
        Point get_added(const Point& p) const;

        void sub(const Point& p);
        Point get_subbed(const Point& p) const;

        void muliply(int val);
        Point get_multipled(int val) const;

        void divide(int val);
        Point get_divided(int val) const;

        void move(Dir dir);
        Point get_moved(Dir dir) const;

        void rotate(Dir dir);
        Point get_rotated(Dir dir) const;

        void set_max(const Point& p);
        void set_min(const Point& p);

        std::string to_string() const;

    public:
        static int s_log_dimemsion;
        static const std::vector<Point> s_dir_8;
        static const std::vector<Point> s_dir_4;

        int x{ 0 };
        int y{ 0 };
        int z{ 0 };
        int w{ 0 };
    };
}