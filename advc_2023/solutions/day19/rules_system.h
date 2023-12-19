#include <map>
#include <memory>
#include <string>
#include <vector>

#include "../../utils/point.h"
#include "ranges.h"

namespace advc_2023::day19
{
    const std::string ACCEPT_LITERAL = "A";
    const std::string REJECT_LITERAL = "R";
    const std::string STARTING_LITERAL = "in";

    // -------------------------------------------

    class Rule
    {
    public:
        enum class cond : uint8_t { none, greater, less };
        enum class sign : uint8_t { positive, negative };
    
        void                parse(const std::string& line);
        bool                is_satisfied(const utils::Point& part) const;
        const std::string   get_destination() const { return m_destination; }
        void                merge_accept_condition(Accept_condition& accept_condition, sign s) const;
    private:
        

        utils::Axis m_part{};
        cond        m_cond{ cond::none };
        short       m_cond_val{};
        std::string m_destination;
    };

    // -------------------------------------------

    class Workflow
    {
    public:
        void                parse(const std::string& line);
        std::string         get_destination(const utils::Point& part) const;
        const std::string   get_name() const { return m_name; }

        const std::string   get_parent() const { return m_parent; }
        void                set_parent(const std::string& parent) { m_parent = parent; }
        Rule                find_rule_to(const std::string& destination) const;

        const std::vector<Rule>& get_rules() const { return m_rules; }

    private:
        std::string         m_name;
        std::string         m_parent;
        std::vector<Rule>   m_rules;
    };

    // -------------------------------------------

    class Rules_system
    {
    public:
        void        parse(const std::vector<std::string>& lines);
        int         get_total_accepted() const;
        uint64_t    get_total_accepted_cases() const;

    private:
        Accept_condition            get_accept_condition(std::shared_ptr<Workflow> workflow, int rule_index) const;
        bool                        is_accepted(const utils::Point part) const;
        std::shared_ptr<Workflow>   get_workflow(const std::string& name) const;

        std::map<std::string, std::shared_ptr<Workflow>>    m_workmap;
        std::vector<std::shared_ptr<Workflow>>              m_accepted_workflows;
        std::vector<utils::Point>                           m_parts;
    };
}