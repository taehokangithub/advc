
#pragma once

#include <assert.h>
#include <limits>
#include <map>
#include "point.h"

namespace advc_2023::utils
{
    constexpr int max_int = std::numeric_limits<int>::max();
    constexpr int min_int = std::numeric_limits<int>::min();

    template <typename T> 
    class Map
    {
    public:
        void set(const Point& p, const T& val);
        bool is_valid_point(const Point& p) const;

        bool exists(const Point& p) const;
        bool exists(int x, int y) const { return exists(Point(x, y)); }
        bool exists(int x, int y, int z) const { return exists(Point(x, y, z)); }

        T get(const Point& p) const;
        T get(int x, int y) const { return get(Point(x, y)); }
        T get(int x, int y, int z) const { return get(Point(x, y, z)); };
        
        const Point& get_min() const { return m_min; }
        const Point& get_max() const { return m_max; }

        const std::map<Point, T> get_data() const { return m_map; }

    private:
        std::map<Point, T> m_map;
        
        Point m_max{ min_int, min_int, min_int, min_int };
        Point m_min{ max_int, max_int, max_int, max_int };
    };

    
    //---------------------------------------------------------------

    template <typename T>
    void Map<T>::set(const Point& p, const T& val)
    {

        m_map[p] = val;
        m_max.set_max(p);
        m_min.set_min(p);
    }
    
    //---------------------------------------------------------------

    template <typename T>
    bool Map<T>::is_valid_point(const Point& p) const
    {
        return p.x >= m_min.x && p.x <= m_max.x &&
            p.y >= m_min.y && p.y <= m_max.y &&
            p.z >= m_min.z && p.z <= m_max.z &&
            p.w >= m_min.w && p.w <= m_max.w;
    }

    //---------------------------------------------------------------

    template <typename T>
    bool Map<T>::exists(const Point& p) const
    {
        const auto ite = m_map.find(p);
        return ite != m_map.end();
    }

    //---------------------------------------------------------------

    template <typename T>
    T Map<T>::get(const Point& p) const
    {
        if (const auto ite = m_map.find(p); ite != m_map.end())
        {
            return ite->second;
        }
        return T();
    }
}