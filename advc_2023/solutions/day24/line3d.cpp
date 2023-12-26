#include <assert.h>
#include <iostream>
#include <sstream>

#include "../../utils/input_utils.h"
#include "../../utils/math.h"
#include "line3d.h"

using namespace std;

namespace advc_2023::day24
{
    Line3D::Line3D(const BigPoint& starting_point, const utils::Point& velocity)
    {
        /*
        * y = ax + b
        * a = vel.y / vel.x
        * b = y - ax
        */
        m_coef_xy.a = (val_type)velocity.y / velocity.x;
        m_coef_xy.b = (val_type)starting_point.y - m_coef_xy.a * starting_point.x;

        m_coef_xz.a = (val_type)velocity.z / velocity.x;
        m_coef_xz.b = (val_type)starting_point.z - m_coef_xz.a * starting_point.x;
    }

    // ---------------------------------------------------------------
    
    Line3D::val_type Line3D::get_y_for_x(val_type x) const
    {
        return m_coef_xy.a * x + m_coef_xy.b;
    }

    // ---------------------------------------------------------------

    Line3D::val_type Line3D::get_z_for_x(val_type x) const
    {
        return m_coef_xz.a * x + m_coef_xz.b;
    }

    // ---------------------------------------------------------------

    Line3D::val_type Line3D::get_x_intersection_xy(const Line3D& rhs) const
    {
        return (rhs.m_coef_xy.b - m_coef_xy.b) / (m_coef_xy.a - rhs.m_coef_xy.a);
    }

    // ---------------------------------------------------------------

    Line3D::val_type Line3D::get_x_intersection_xz(const Line3D& rhs) const
    {
        return (rhs.m_coef_xz.b - m_coef_xz.b) / (m_coef_xz.a - rhs.m_coef_xz.a);
    }

    // ---------------------------------------------------------------

    BigPoint Line3D::get_intersection(const Line3D& rhs) const
    {
        const auto x = get_x_intersection_xy(rhs);
        const auto y = get_y_for_x(x);
        const auto z = get_z_for_x(x);

#ifdef _DEBUG
        const auto another_x = get_x_intersection_xz(rhs);
        const auto another_y = rhs.get_y_for_x(another_x);
        const auto another_z = rhs.get_z_for_x(another_x);

        assert(utils::is_almost_same(x, another_x));
        assert(utils::is_almost_same(y, another_y));
        assert(utils::is_almost_same(z, another_z));
#endif

        BigPoint p{ (int64_t)std::round(x), (int64_t)std::round(y), (int64_t)std::round(z) };

        assert(utils::is_almost_same((double)p.x, x));
        assert(utils::is_almost_same((double)p.y, y));
        assert(utils::is_almost_same((double)p.z, z));

        return p;
    }

    // ---------------------------------------------------------------

    bool Line3D::has_intersection(const Line3D& rhs) const
    {
        const auto x = get_x_intersection_xy(rhs);
        const auto y = get_y_for_x(x);
        const auto z = get_z_for_x(x);

        const auto another_x = get_x_intersection_xz(rhs);
        const auto another_y = rhs.get_y_for_x(another_x);
        const auto another_z = rhs.get_z_for_x(another_x);

        return utils::is_almost_same(x, another_x)
            && utils::is_almost_same(y, another_y)
            && utils::is_almost_same(z, another_z);
    }

}