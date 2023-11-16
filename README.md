# Split sample app walkthrough

## Prerequisites

1. Create a Split.io account
2. Create a Split.io organization
3. Create a workspace (or use the default workspace) under the organization.

## Create feature flags

The sample web app uses three feature flags:
1. new_cover_photo:

    - Feature: show a new cover photo.
    - Treatment: on (50%) / off (50%)

2. survey_incentive: 

    - Feature: show a new survey description with more incentive for survey participation.
    - Treatment: on (50%) / off (50%)

3. large_cover_photo: 

    - Feature: increase the size of the cover photo.
    - Treatment: on (50%) / off (50%)
    - Dynamic configuration: on (photo height 400 pixels) / off (photo height 150 pixels)

Login [Split portal](https://app.split.io/) and creates the feature flags following the below configuration.

### Feature flag: new_cover_photo

Treatments:

![new_cover_photo treatments](images/feature_flag_new_cover_photo_1.png)

Targeting rules:

![new_cover_photo targeting rules](images/feature_flag_new_cover_photo_2.png)

### Feature flag: survey_incentive

Treatments:

![survey_incentive treatments](images/feature_flag_survey_incentive_1.png)

Targeting rules:

![survey_incentive targeting rules](images/feature_flag_survey_incentive_2.png)

### Feature flag: large_cover_photo

Treatments:

![large_cover_photo treatments](images/feature_flag_large_cover_photo_1.png)

Dynamic configuration:

![large_cover_photo targeting rules](images/feature_flag_large_cover_photo_2.png)

Targeting rules:

![large_cover_photo targeting rules](images/feature_flag_large_cover_photo_3.png)

## Run the sample web app

The sample web app uses Split [.NET server-side SDK](https://help.split.io/hc/en-us/articles/360020240172--NET-SDK) to retrieve feature flags from Split and emit events to Split. It requires a Split server-side API key to authenticate with Split API.

From `Admin settings`, copy the server-side SDK API key corresponding to the same environment where the feature flags are created.

![Server-side SDK API key](images/server_side_api_key.png)

Save the API key in a secret file under the path `%APPDATA%\Microsoft\UserSecrets\a3e3f50b-3bee-4110-b6a5-d57e5ff7c17c\secrets.json` with the following format:

```
{
  "Splitio:ApiKey": "<Split_API_Key>"
}
```
Run the solution to launch both the sample web app and the test program that mocks users and sends traffic to the sample web app. The impressions and events start to be sent to Split from the app.

The app sents an event to Split for each survey participation. Each event includes `eventTypeId` as `image_rating`, `key` as the user ID, `value` as the score the user rates the web page, `timestamp` and other required metadata to attribute it to impressions.

## Create metrics

Two metrics are created to evaluation the impact of the feature flags

![survey score metric definition](images/metrics_survey_score.png)

![survey participation metric definition](images/metrics_survey_participation.png)

## View experimentation results

## Query impressions and events from Data Hub
