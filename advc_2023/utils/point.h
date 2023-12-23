
#pragma once

#include <string>
#include <vector>

namespace advc_2023::utils
{
    enum class Dir : uint8_t { Up, Right, Down, Left };
    enum class Axis : uint8_t { X, Y, Z, W };
    
    struct Point
    {
    public:
        Point() {}
        Point(int _x, int _y) : x(_x), y(_y) {}
        Point(int _x, int _y, int _z) : x(_x), y(_y), z(_z) {}
        Point(int _x, int _y, int _z, int _w) : x(_x), y(_y), z(_z), w(_w) {}
        Point(const Point& other);
        Point(const std::string& str) { *this = unique_string_to_point(str); }

        bool operator==(const Point& p) const;
        bool operator!=(const Point& p) const;
        bool operator<(const Point& p) const;

        int get(Axis axis) const;
        void set(Axis axis, int val);
        int add_all_axis() const;

        void add(const Point& p);
        Point get_added(const Point& p) const;

        void sub(const Point& p);
        Point get_subbed(const Point& p) const;

        void muliply(int val);
        Point get_multipled(int val) const;

        void divide(int val);
        Point get_divided(int val) const;

        void move(Dir dir, int distance = 1);
        Point get_moved(Dir dir, int distance = 1) const;

        void rotate(Dir dir);
        Point get_rotated(Dir dir) const;

        int get_manhattan_distance(const Point& rhs) const;

        void set_max(const Point& p);
        void set_min(const Point& p);

        std::string to_string() const;          // for debugging purpose
        std::string to_string_3d() const;         
        std::string to_unique_string() const;   // for being keys in a map

        static Point unique_string_to_point(const std::string& str);
        static Point parse_from_comma_string(const std::string& str);
        static Dir get_opposite_dir(Dir dir);
        static Dir get_rotated_dir(Dir dir, Dir rotate);
        static std::string get_dir_name(Dir dir) { return s_dir_names[(int)dir]; }

    public:
        static const std::vector<Point> s_dir_8;
        static const std::vector<Point> s_dir_4;
        static const std::vector<Dir> s_dirs;
        static const std::vector<std::string> s_dir_names;
        

        int x{ 0 };
        int y{ 0 };
        int z{ 0 };
        int w{ 0 };
    };
}