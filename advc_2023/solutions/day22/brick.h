#include <map>
#include <memory>
#include <string>
#include <unordered_map>
#include <unordered_set>
#include <vector>

#include "../../utils/point.h"

namespace advc_2023::day22
{
    struct Brick
    {
    public:
        bool check_horizontal_collision(const Brick& other) const;    // true == collision
        void drop_down();

        std::string to_string() const;
        bool is_vertical() const { return lower.z != upper.z; }
    public:
        utils::Point lower, upper;
    };

    class Brick_map
    {
        using brick_set_type = std::unordered_set<std::shared_ptr<Brick>>;

    public:
        void parse(const std::vector<std::string>& lines);
        int count_disintegratable_bricks() const;
        int count_chain_reactions() const;

    private:
        void free_fall_all();
        void free_fall(std::shared_ptr<Brick> brick);

        void add_to_floor(std::shared_ptr<Brick> brick);
        void remove_from_floor(std::shared_ptr<Brick> brick);

        void chain_reaction_internal(std::shared_ptr<Brick> brick, brick_set_type& chained_away) const;

        std::map<int, brick_set_type> m_floors;
        std::unordered_map<std::shared_ptr<Brick>, brick_set_type> m_supports;
        std::unordered_map<std::shared_ptr<Brick>, brick_set_type> m_supported_by;
    };
}