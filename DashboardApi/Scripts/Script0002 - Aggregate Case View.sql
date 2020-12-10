create or replace view covid19_cases_jk_aggregate_view as
select country_region,
       date_trunc('day', last_update)                                                                               as entry_date,
       sum(confirmed)                                                                                               as confirmed_today,
       sum(deaths)                                                                                                  as deaths_today,
       sum(recovered)                                                                                               as recovered_today,
       sum(confirmed) - lag(sum(confirmed), 1, 0::bigint)
                        over (partition by country_region order by date_trunc('day', last_update))                  as confirmed_change,
       sum(deaths) - lag(sum(deaths), 1, 0::bigint)
                     over (partition by country_region order by date_trunc('day', last_update))                     as deaths_change,
       sum(recovered) - lag(sum(recovered), 1, 0::bigint)
                        over (partition by country_region order by date_trunc('day', last_update))                  as recovered_change
from covid19_cases_jh
group by entry_date, country_region;