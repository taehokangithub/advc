#include <memory>
#include <string>
#include <vector>

#include "../../utils/Point.h"

namespace advc_2023::day24
{
    struct BigPoint
    {
        int64_t x{}, y{}, z{};
    };

    // -------------------------------------------------------------------------

    class Line3D
    {
    public:
        using val_type = double;
        
        struct coef_2d // 2D coefficents of a line [y = ax + b]
        {
            val_type a{ 0 };
            val_type b{ 0 };
        };

    public:
        Line3D() = default;
        Line3D(const BigPoint& starting_point, const utils::Point& velocity);

        val_type get_y_for_x(val_type x) const;
        val_type get_z_for_x(val_type x) const;

        const coef_2d& get_coef_xy() const { return m_coef_xy; }
        const coef_2d& get_coef_xz() const { return m_coef_xz; }

        // non-const methods
        coef_2d& get_coef_xy() { return m_coef_xy; }
        coef_2d& get_coef_xz() { return m_coef_xz; }

        val_type get_x_intersection_xy(const Line3D& rhs) const;
        val_type get_x_intersection_xz(const Line3D& rhs) const;

        BigPoint get_intersection(const Line3D& rhs) const;
        bool has_intersection(const Line3D& rhs) const;

    private:
        coef_2d       m_coef_xy;   // Coefs for the projection line XY plane (y = ax + b)
        coef_2d       m_coef_xz;   // Coefs for the projection line XZ plane (z = ax + b)
    };
}