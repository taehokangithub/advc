#include <memory>
#include <string>
#include <vector>

#include "../../utils/Point.h"
#include "line3d.h"

namespace advc_2023::day24
{
    class Hailstone
    {

    public:
        Hailstone() = default;
        Hailstone(const BigPoint& starting_point, const utils::Point& velocity) : m_location(starting_point), m_velocity(velocity) {}

        void        parse(const std::string& line);
        std::string to_string() const;

        bool        does_intersect_2d(const Hailstone& rhs, uint64_t from, uint64_t to) const;
        Line3D      get_line3d() const { return Line3D(m_location, m_velocity); }
        int64_t     get_position_sum() const { return m_location.x + m_location.y + m_location.z; }

        const BigPoint&     get_location() const { return m_location; }
        const utils::Point& get_velocity() const { return  m_velocity; }

    private:
        BigPoint     m_location;
        utils::Point m_velocity;
    };

    // ------------------------------------------------------

    class Hailstones
    {
    public:
        void        parse(const std::vector<std::string>& lines);
        int         count_all_intersections_2d(uint64_t from, uint64_t to) const;
        int64_t     get_passer_initial_position() const;

    private:
        Line3D      get_passer() const;
        int64_t     get_position_sum_from_passer(const Line3D& passer) const;

        std::vector<std::shared_ptr<Hailstone>> m_hailstones;
    };
}