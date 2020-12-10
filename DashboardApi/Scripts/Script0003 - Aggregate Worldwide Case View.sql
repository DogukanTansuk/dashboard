create or replace view covid19_cases_jk_aggregate_worldwide_view
as
select date_trunc('day'::text, covid19_cases_jh.last_update)                                                     as entry_date,
       sum(covid19_cases_jh.confirmed)                                                                           as confirmed_today,
       sum(covid19_cases_jh.deaths)                                                                              as deaths_today,
       sum(covid19_cases_jh.recovered)                                                                           as recovered_today,
       sum(covid19_cases_jh.confirmed) - lag(sum(covid19_cases_jh.confirmed), 1, 0::bigint)
                                         over (order by (date_trunc('day'::text, covid19_cases_jh.last_update))) as confirmed_change,
       sum(covid19_cases_jh.deaths) - lag(sum(covid19_cases_jh.deaths), 1, 0::bigint)
                                      over (order by (date_trunc('day'::text, covid19_cases_jh.last_update)))    as deaths_change,
       sum(covid19_cases_jh.recovered) - lag(sum(covid19_cases_jh.recovered), 1, 0::bigint)
                                         over (order by (date_trunc('day'::text, covid19_cases_jh.last_update))) as recovered_change
from covid19_cases_jh
group by (date_trunc('day'::text, covid19_cases_jh.last_update));