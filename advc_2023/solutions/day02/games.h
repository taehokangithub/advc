#include <map>
#include <memory>
#include <string>
#include <vector>

using namespace std;
namespace advc_2023::day02
{
    enum class Colour
    {
        blue, red, green, max
    };

    struct Element
    {
        Colour colour;
        int value;
    };

    using Filter_type = map<Colour, int>;

    // ----------------------------------------

    class Draw
    {
    public:
        void parse(const string& contents);
        bool is_passed(const Filter_type& filter) const;
        const vector<Element> get_elements() const { return m_elements; }

    private:
        vector<Element> m_elements;
    };

    // ----------------------------------------

    class Game
    {
    public:
        void parse(const string& line);
        bool is_passed(const Filter_type& filter) const;
        int get_id() const { return  m_id; }
        int get_power() const;

    private:
        int m_id;
        vector<shared_ptr<Draw>> m_draws;
    };

    // ----------------------------------------

    class Games
    {
    public:
        void parse(const vector<string>& lines);
        int get_sum_of_passed_games(const Filter_type& filter) const;
        int get_sum_of_powers() const;

    private:
        vector<shared_ptr<Game>> m_games;
    };
}

