
#pragma once

#include <assert.h>
#include <vector>
#include "point.h"

namespace advc_2023::utils
{
    template <typename T>
    class Grid
    {
    public:
        void set_size(const Point& p);
        void set_size(int x, int y) { set_size(Point(x, y)); }
        const Point& get_size() const { return m_size; }

        Point add_element(const T& val);
        void set(const Point& p, const T& val);
        bool is_all_set() const;
        bool is_valid_point(const Point& p) const;
        bool is_valid_point(int x, int y) const { return is_valid_point(Point(x, y)); }

        const T& get(const Point& p) const;
        const T& get(int x, int y) const { return get(Point(x, y)); }

    private:
        std::vector<std::vector<T>> m_grid;
        Point m_size;
        Point m_add_point;
    };

    // -------------------------------------------------

    template <typename T>
    void Grid<T>::set_size(const Point& p)
    {
        for (int i = 0; i < p.y; i++)
        {
            m_grid.push_back(std::vector<T>());
            for (int k = 0; k < p.x; k++)
            {
                m_grid[i].push_back(T());
            }
        }
        m_size = p;
    }

    // -------------------------------------------------

    template <typename T>
    Point Grid<T>::add_element(const T& val)
    {
        Point ret = m_add_point;

        set(m_add_point, val);

        m_add_point.x++;
        assert(m_add_point.x <= m_size.x);

        if (m_add_point.x == m_size.x)
        {
            m_add_point.x = 0;
            m_add_point.y++;
            assert(m_add_point.y <= m_size.y);
        }

        return ret;
    }

    // -------------------------------------------------

    template <typename T>
    const T& Grid<T>::get(const Point& p) const
    {
        return m_grid[p.y][p.x];
    }

    // -------------------------------------------------

    template <typename T>
    void Grid<T>::set(const Point& p, const T& val)
    {
        m_grid[p.y][p.x] = val;
    }

    // -------------------------------------------------

    template <typename T>
    bool Grid<T>::is_all_set() const
    {
        return (m_add_point.x == 0 && m_add_point.y == m_size.y);
    }

    // -------------------------------------------------

    template <typename T>
    bool Grid<T>::is_valid_point(const Point& p) const
    {
        return p.x >= 0 && p.x < m_size.x && p.y >= 0 && p.y < m_size.y;
    }
}