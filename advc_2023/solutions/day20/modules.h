#include <unordered_map>
#include <string>
#include <vector>
#include <queue>


namespace advc_2023::day20
{
    enum class signal : uint8_t { none, high, low };

    struct signal_sent {
        const std::string& source;
        const std::string& target;
        signal signal;
    };

    class Module
    {
    public:
        virtual bool                    is_original_state() const = 0;
        virtual signal                  process_input(const signal_sent& input_signal) = 0;

        const std::vector<std::string>& get_outputs() const { return m_outputs; }
        void                            add_output(const std::string& s) { m_outputs.push_back(s); }
        virtual void                    add_input(const std::string& s) {}

        void                            set_name(const std::string& n) { m_name = n; }
        const                           std::string& get_name() const { return m_name; }
        virtual                         std::string get_type_name() const = 0;

    private:
        std::vector<std::string> m_outputs;
        std::string m_name;
    };

    // --------------------------------------------------

    class Broadcaster : public Module
    {
    public:
        bool        is_original_state() const override { return true; }
        signal      process_input(const signal_sent& input_signal) override { return input_signal.signal; }
        std::string get_type_name() const { return "Broadcaster"; }
    };

    // --------------------------------------------------

    class Flipflop : public Module
    {
    public:
        bool        is_original_state() const { return !m_state; }
        signal      process_input(const signal_sent& input_signal) override;
        std::string get_type_name() const { return "Flipflop"; }

    private:
        bool m_state{ false };
    };

    // --------------------------------------------------

    class Conjunction : public Module
    {
    public:
        bool        is_original_state() const;
        signal      process_input(const signal_sent& input_signal) override;
        void        add_input(const std::string& s) override { m_inputs[s] = signal::low; }
        std::string get_type_name() const { return "Conjunction"; }

        const std::unordered_map<std::string, signal> get_inputs() const { return m_inputs; }
    private:
        signal  get_input_state(const std::string& input) const { return m_inputs.find(input)->second; }   // let it crash on non-existing key

        std::unordered_map<std::string, signal> m_inputs;
    };

    // --------------------------------------------------

    class Modules_manager
    {
    public:
        void        parse(const std::vector<std::string>& lines);
        int         get_total_signal_score(int total_loops) const;
        uint64_t    get_cnt_button_pressed(std::string target) const;

    private:
        bool    is_original_state() const;
        void    handle_sent_signal(std::queue<signal_sent>& q, const signal_sent ss) const;

        std::unordered_map<std::string, std::shared_ptr<Module>> m_modules;
    };
}