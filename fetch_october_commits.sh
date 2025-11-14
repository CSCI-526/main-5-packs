#!/bin/bash

# Script to fetch commit links from the last week of October 2025
# Last week of October: October 24-31, 2025

REPO_URL="https://github.com/CSCI-526/main-5-packs"
START_DATE="2025-10-24"
END_DATE="2025-11-01"  # Exclusive end date

# Parse command line arguments
AUTHOR=""
FORMAT="default"

while [[ $# -gt 0 ]]; do
    case $1 in
        --author)
            AUTHOR="$2"
            shift 2
            ;;
        --format)
            FORMAT="$2"
            shift 2
            ;;
        --help)
            echo "Usage: $0 [OPTIONS]"
            echo ""
            echo "Fetch commit links from the last week of October 2025 (Oct 24-31)"
            echo ""
            echo "Options:"
            echo "  --author NAME    Filter commits by author name or email"
            echo "  --format FORMAT  Output format: default, markdown, json (default: default)"
            echo "  --help           Show this help message"
            echo ""
            echo "Examples:"
            echo "  $0                           # Get all commits"
            echo "  $0 --author john.doe         # Get commits by john.doe"
            echo "  $0 --format markdown         # Output as markdown list"
            exit 0
            ;;
        *)
            echo "Unknown option: $1"
            echo "Use --help for usage information"
            exit 1
            ;;
    esac
done

# Build git log command
GIT_CMD="git log --all --pretty=format:'%H|%an|%ae|%ad|%s' --date=iso --since=\"$START_DATE\" --until=\"$END_DATE\""

# Add author filter if specified
if [ -n "$AUTHOR" ]; then
    GIT_CMD="$GIT_CMD --author=\"$AUTHOR\""
fi

# Execute git command and process output
COMMITS=$(eval $GIT_CMD)

if [ -z "$COMMITS" ]; then
    echo "No commits found in the last week of October 2025 (Oct 24-31)"
    if [ -n "$AUTHOR" ]; then
        echo "Author filter: $AUTHOR"
    fi
    exit 0
fi

# Count commits
COMMIT_COUNT=$(echo "$COMMITS" | wc -l)

# Output header
echo "====================================================================="
echo "Commits from Last Week of October 2025 (October 24-31, 2025)"
if [ -n "$AUTHOR" ]; then
    echo "Filtered by author: $AUTHOR"
fi
echo "Total commits found: $COMMIT_COUNT"
echo "====================================================================="
echo ""

# Process each commit
case $FORMAT in
    json)
        echo "["
        FIRST=1
        while IFS='|' read -r sha author email date message; do
            if [ $FIRST -eq 0 ]; then
                echo ","
            fi
            FIRST=0
            echo "  {"
            echo "    \"sha\": \"$sha\","
            echo "    \"author\": \"$author\","
            echo "    \"email\": \"$email\","
            echo "    \"date\": \"$date\","
            echo "    \"message\": \"$message\","
            echo "    \"url\": \"$REPO_URL/commit/$sha\""
            echo -n "  }"
        done <<< "$COMMITS"
        echo ""
        echo "]"
        ;;
    markdown)
        while IFS='|' read -r sha author email date message; do
            echo "- [$message]($REPO_URL/commit/$sha) - $author ($date)"
        done <<< "$COMMITS"
        ;;
    *)
        while IFS='|' read -r sha author email date message; do
            echo "Commit: $message"
            echo "Author: $author <$email>"
            echo "Date: $date"
            echo "Link: $REPO_URL/commit/$sha"
            echo "---------------------------------------------------------------------"
        done <<< "$COMMITS"
        ;;
esac
